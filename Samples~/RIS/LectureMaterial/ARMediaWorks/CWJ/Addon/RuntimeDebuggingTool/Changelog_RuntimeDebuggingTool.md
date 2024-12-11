
### RuntimeDebuggingTool Changelog

###	━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
###	=============== Copyright 2019. CWJ All rights reserved ===============
###	━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

##	Contact : cwj@kakao.com (조우정)

##	TODO
	1. FPSCounter 로그에서 사양은 제일 위로 올리고 매 실행 시 마다 입력되지않게

##	Version
##  6
	1) LogItem을 클릭후, 클릭된 LogItem을 길게 클릭하거나 우클릭 시 클립보드에 복사됨.
	2) RuntimeDebuggingTool.Instance.AddSavingLog() 로 끌때 저장되는 CWJ_ProfilingLog.txt파일내용에 추가로 로그를 저장시킬수 있음
	3) CWJ_ProfilingLog.txt 내용 최적화시킴 (디바이스 스탯을 최초 한번만 최상단에 기입시킴으로써 매번 중복작성되어 내용이 많은 불편사항 해결)

##	5
	1) Debugging

##	4.2
	1) Add function to disable RuntimeDebuggingTool (In UnityDevTool)

##	4.1
	1) Fixed bug about singleton, DontDestroyOnLoad 

##	4.0
	1) StandAlone. (CWJ - Developer-friendlyPackage 와 분리)
	2) 폴더 구조 변경

##	3.3
	SetScriptExecutionOrder 적용

##	3.2
	FixedFrame 관련 버그때문에 기능 비활성화