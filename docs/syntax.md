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
    pub x: int,
    pub y: int,
}

type Pointable = Point | ()
```

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
}

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

## 매크로

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
}

// invalid
#assert(with_side_effects() > 0);
```

대신 다음과 같은 코드는 컴파일된다.

```zoa
// valid, because is_valid function is pure
let is_valid = (a) { a > 0 }
#assert((1..10).map(is_valid).all());

// valid
#assert({
    let a = 10;
    let with_side_effects = () {
        a++
    }

    with_side_effects() > 10
})
```
