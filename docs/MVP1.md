# MVP

1차 MVP 로 가볍게 컴파일되는 zoa의 프로토타입 정도를 만들어본다.

## 목표

메모리 할당, 구조체, 트레잇과 기본적인 스탠다드 타입을 사용할 수 있고 네이티브 컴파일이 되야 함. 그리고 구문 분석에 힘을 쏟기

```zoa
import std;

main = () {
    let size = int.parse(std.stdio.readline())
        .expect("invalid input");
    let arr: [int] = [int].alloc(size);

    // do something cool

    arr.free();
}
```

와 뭘 어떻게 해도 못생겼네... 메모리는 진짜 어떻게 해야겠다.. 일단 이렇게라도 구현을 해보고 그다음 뭐라도 하자
