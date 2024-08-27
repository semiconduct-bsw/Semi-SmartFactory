-- 실습 18-1 SELECT INTO 사용한 단일행 데이터 저장
DECLARE
    V_DEPT_ROW DEPT%ROWTYPE;
BEGIN
    SELECT DEPTNO, DNAME, LOC INTO V_DEPT_ROW
        FROM DEPT WHERE DEPTNO = 40;
    DBMS_OUTPUT.PUT_LINE('DEPTNO : ' || V_DEPT_ROW.DEPTNO);
    DBMS_OUTPUT.PUT_LINE('DNAME : ' || V_DEPT_ROW.DNAME);
    DBMS_OUTPUT.PUT_LINE('LOC : ' || V_DEPT_ROW.LOC);
END;
/

-- 실습 18-2 단일행 데이터 커서 사용
DECLARE
    -- 커서 데이터를 입력할 변수 선언
    V_DEPT_ROW DEPT%ROWTYPE;
    
    -- 명시적 커서 선언(Declaration)
    CURSOR C1 IS SELECT DEPTNO, DNAME, LOC
    FROM DEPT WHERE DEPTNO = 40;
BEGIN
    -- 커서 열기(OPEN)
    OPEN C1;
    -- 커서로부터 읽어온 데이터 사용(FETCH)
    FETCH C1 INTO V_DEPT_ROW;
    DBMS_OUTPUT.PUT_LINE('DEPTNO : ' || V_DEPT_ROW.DEPTNO);
    DBMS_OUTPUT.PUT_LINE('DNAME : ' || V_DEPT_ROW.DNAME);
    DBMS_OUTPUT.PUT_LINE('LOC : ' || V_DEPT_ROW.LOC);
    -- 커서 닫기
    CLOSE C1;
END;
/

-- 실습 18-8 예외 발생 PL/SQL
-- 에러가 발생하는 코드
DECLARE
    V_WRONG NUMBER;
BEGIN
    SELECT DNAME INTO V_WRONG FROM DEPT WHERE DEPTNO = 10;
END;
/

-- EXCEPTION을 통해서 예외처리
DECLARE
    V_WRONG NUMBER;
BEGIN
    SELECT DNAME INTO V_WRONG FROM DEPT WHERE DEPTNO = 10;
EXCEPTION
    WHEN VALUE_ERROR THEN
        DBMS_OUTPUT.PUT_LINE('에러발생!');
END;
/

DECLARE
  -- 사용자 정의 예외 선언
  e_high_salary EXCEPTION;
  -- 급여 한도 선언
  salary_limit CONSTANT NUMBER := 5000;
  -- 직원의 급여를 저장할 변수
  v_salary EMP.SAL%TYPE;
  v_ename EMP.ENAME%TYPE;
BEGIN
  -- 특정 직원의 급여 조회 (예: EMPNO가 7839인 경우)
  SELECT SAL, ENAME INTO v_salary, v_ename
  FROM EMP
  WHERE EMPNO = 7839;

  -- 조건: 급여가 5000 이상일 때 예외 발생
  IF v_salary >= salary_limit THEN
    RAISE e_high_salary; -- 예외 발생
  END IF;

  -- 예외가 발생하지 않았을 경우의 처리
  DBMS_OUTPUT.PUT_LINE(v_ename || '의 급여는 ' || v_salary || '입니다.');
EXCEPTION
  -- 사용자 정의 예외 처리
  WHEN e_high_salary THEN
    DBMS_OUTPUT.PUT_LINE(v_ename || '의 급여는 ' || v_salary || '로, 허용된 한도를 초과했습니다.');
  -- 다른 예외 처리 (옵션)
  WHEN OTHERS THEN
    DBMS_OUTPUT.PUT_LINE('다른 오류가 발생했습니다: ' || SQLERRM);
END;
/
