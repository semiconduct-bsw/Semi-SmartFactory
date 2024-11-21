### SampleLog

``` //ID Report
2023-01-17 16:06:02.703
'10701' - RPTID
	'MODEL' - MODELID
	'Operator' - OPID
	'Process' - PROCID
	'Material' - MaterialID

							
//Started Report
2023-01-17 16:06:02.703
'10703' - RPTID
	'MODEL' - MODELID
	'Operator' - OPID
	'Process' - PROCID
	'Material' - MaterialID
	'LOT' - LOTID

							
//Complete Report
2023-01-17 16:06:02.703 
'10713' RPTID
	'MODEL' - MODELID
	'Operator' - OPID
	'Process' - PROCID
	'Material' - MaterialID
	'LOT' - LOTID
	'SENSOR0Value' - SENSOR0
	'SENSOR1Value' - SENSOR1
	'SENSOR2Value' - SENSOR2
	'OK' - JUDGE							

// START(RCMD)
2023-01-17 16:06:03.412 
'START' RCMD
	'MODEL' - MODELID
	'MaterialID' - MaterialID
	'Process' - PROCID
	'LOT' - LOTID
							    ```

### 2주차 과제 (GUI 에 너무 힘쓰지 말 것.)

```
1.	CIM PC & MES PC GUI 제작
-	CIM PC
	MES PC와 PLC PC와 통신 상태 표출 기능 포함 (Label).
	Online Remote Report 전송 보내는 버튼 제작 (장비를 통하지 않고 CIM  MES만 수행)
	Log 확인할 수 있는 화면 제작(ex. ListBox)
	금일 생산한 제품 데이터 출력(GridView 활용)
-	MES PC
	DB에 있는 데이터를 조회할 수 있는 화면 구현 datagridview
	날짜별로 생산한 제품 조회 검색 버튼 추가
	Barcode로 제품 조회 검색 버튼 추가
	심화. DB를 날짜별로 검색했을 시 OK or NG를 구분해서 조회 버튼 추가
	Log 확인할 수 있는 화면 제작(ex.ListBox) 로그 데이터 파일로 저장

CIM, MES 공통
-	SECS GEM 통신 로그를 파일로 저장할 수 있는 기능 구현.
-	SampleLog 파일을 참고해서 수신, 송신 발생 시 Report와 RCMD Send 로그 구현.
-	경로 : 프로젝트 exe파일 경로\\log\\오늘날짜.log로 누적하여 저장, 저장 시점에 listview 에 로그 내용을 추가하며 저장하기(표출, 저장 동시에)

2.	제품 테스트 시작부터 완료까지 메인 시퀀스 Switch Case 문으로 시퀀스 작성
-	송수신 데이터 구조체에 데이터 저장 후 시나리오에 맞춰 데이터 송수신.
3.	기구물 제작

4.	장비  프로그램.
-	랜덤 바코드 생성 규칙 yyyymmddhhmmss_1 로 바코드 생성 (_1 은 모델)
-	해당 바코드를 기반으로 _1 이 모델 1번 임을 MES 에서 인지


전체 공통
Online report -> material id report -> RCMD – START -> started Report 
까지 이어지는 시나리오를 장비 -> CIM -> MES 상호간 수행하도록 완료

추가로 위 시퀀스 완료되면 
장비 에서 완료 신호 발생 시 completed report 를 CIM -> MES 를 통해 전송 후 완공 처리

및 장비 초기상태로 원상복구.
	장비가 완성 전이면 완료 시 시작, 정지, 센서값 데이터를 전송.


							    ```
