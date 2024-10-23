아래 두 모듈을 설치

먼저 colab 메뉴의 runtime → Change runtime type을 선택해서 GPU를 선택 (T4 GPU, 12시간 제한 있음)

!pip3 install torch

!pip3 install torchvision

작성 후에 Ctrl + Enter를 통해 코드 실행 가능 ( ! = Linux 명령어 )

import torch
x = torch.Tensor(3, 4).cuda()
print(x)

로 테스트
