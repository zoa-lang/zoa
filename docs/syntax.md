# 신택스

아직 구상중.. 기본적으로 러스트의 플로우를 따르고 있음. 넣고 싶은 힙한 신택스는 많지만 오히려 방해가 될까봐 고민중..

신택스는 가독성을 해하지 않고 적당히 익숙하게 디자인하도록 한다. 언어의 문법에 대해 전혀 모르는 사람이 코드를 보고 코드의 시멘틱을 이해할 수 있어야 한다.

## 코드 플로우

조건

```zoa
let res = if a {
    1+1
}
else if
    1
else {
    3
}

match (x, y) {
    (3, _) -> 3,
    a -> 10,
}
```

반복

```zoa
for i in 1..10 {

}

while !true {

}


```

## Iter

```zoa
// exclusive range
let r = 1..10;
// inclusive range
let r = 1..=10;
// step
let r = (1..10).step_by(2);

// without sugar
let r = ExclusiveRange.new(1, 10);
let r = InclusiveRange.new(1, 10);
```

## 타입

구조체와 타입 얼라이어스가 있다. 구조체는 타입 하위호완

```zoa
struct Point {
    pub x: int = 10;
    pub y: int;

    pub sta new = (x, y) -> Point {
        Point { x, y }
    };

    pub sta from_tuple = (x: (int, int)) -> Point {
        new(x.0, x.1)
    };

    pub as_tuple = () -> (int, int) {
        (self.x, self.y)
    };
}

let p0 = Point.new(0, 0);
let p1 = Point.from(p0.as_tuple());
println($"({p1.x}, {p1.y})");

type Pointable = Option<Point>
```

static 메서드와 멤버 메서드를 구분할 방법이 없어서 `static` 키워드를 추가해봤지만 너무 못생겨서 취소했다. 하지만 이렇게 된다면 static한 필드를 어떻게 나타낼 것인가에 대해 생각해보아야 한다.

인터페이스와 위임은 다음처럼

```zoa
// all fileds are public
trait Organism {
    name: str;
    
    sta new: () -> Self;
    say_name: () -> ();
}

struct Human : Organism {
    pub name: str;

    pub sta new = () -> Human {
        new("")
    }

    pub sta new = (name: str) -> Human {
        Human { name }
    }

    pub say_name = () {
        println($"my name is {self.name}!!!");
    };
}

let org: <Organism> = Human;
let human = org.new();
human.say_name();
```

trait와 struct을 구분할 수 있는 컨벤션이 있으면 좋겠다. TODO
enum의 멤버 메서드에 대해 생각 안했다. TODO (확장 메서드 기능 사용?)

원래는 두개 이상의 타입 A, B 등에 대해서 `A | B` 와 같은 타입을 생각했었는데 `match` 신택스를 사용한 타입 매칭에서 어떻게 해야 할지 잘 모르겠더라.

이에 대해 생각한걸 좀만 더 말해보자면, 
`A | B` 와 같은 형태는 `enum` 과 방식은 같지만 좀 더 위험하고 불편한 타입이라고 생각되었다. 생각했던 두개의 모양은 다음과 같다.

```
type Optional<T> = T | ();
```

```
enum Optional<T> {
    Some(T),
    None,
}
```

두 방식을 비교해보자. `enum` 같은 경우에는 새로운 타입을 만들지만 `|` 연산자를 사용한 경우는 있는 타입들을 묶을 뿐이다. 새로운 타입을 만드는게 타입을 define 해야 한다는 점에서 조금 귀찮을 수 있지만 표현력이 더 뛰어나다. 겉으로는 둘은 비슷하지만 (return 할 때 두개 이상의 타입을 리턴해도 된다는 건 비슷하다, 의미도 비슷하다) 둘은 전혀 다른 코드이다.

## 변수

변수는 기본적으로 mutable 함. 변수 선언에는 보통 타입 추론이 됨.

```zoa
let a = 3; // a: int
let a: int = 3; // a: int
```

## 함수

 - 함수의 리턴 타입을 알기 쉬워야 함
 - 멤버 함수와 익명 함수의 선언 신택스가 같아야 함

```zoa
let add = (a: int, b: int) -> int {
    a + b
};

[1, 2, 3].fold(0, (a, b) { a + b });
[1, 2, 3].fold(0, (a, b) -> int | () { a + b });
```

## 모듈

```zoa
import std; // std.stdin()
import std.serialize.*;
import std.io.File as F;
import std.io.{StreamReader, StreamWriter};
```

모듈 선언: 파일 맨 앞에? 혹은 블럭으로?

```zoa
mod A {
    struct B {

    }
}
```

```zoa
mod A;

struct B {

}
```

## [매크로](/docs/macro.md)

> 아래 설명은 이제 맞는 말이 아닙니다^^ 그치만 다음에 참고하게끔 남겨두겟음 클로져 없는 컴파일타임 어썰션은 큰 의미가 없다

컴파일 타임 코드 치환 가장 기본적인 실행 `#run` 매크로

```zoa
let compile_time_random_value = #run {
    rand()
};
```

컴파일 타임 어썰션 `#assert` 매크로

```zoa
#assert_eq(3, i);
```

물론 run이나 assert 매크로 안에 클로져와 같은 바깥 변수 캡쳐는 허용되지 않는다. 매크로 파라미터에 재귀적으로 코드를 검사해, 사이드 이펙트가 있는 코드가 포함되면 컴파일이 되지 않는다. 다음과 같은 코드는 컴파일되지 않는다.

```zoa
// invalid
for i in 1..10 {
    #assert(i > 0);
}

let a = 10;
let with_side_effects = () {
    a++
};

// invalid
#assert(with_side_effects() > 0);
```

대신 다음과 같은 코드는 컴파일된다.

```zoa
// valid, because is_valid function is pure
let is_valid = (a) { a > 0 };
#assert((1..10).map(is_valid).all());

// valid
#assert({
    let a = 10;
    let with_side_effects = () {
        a++
    };

    with_side_effects() > 10
})
```
