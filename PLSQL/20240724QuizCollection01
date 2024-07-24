-- Q1) 1~10 사이의 3의 배수만 출력하기
-- 출력 : 이라는 말을 앞에 붙혀서 출력하게 하기 (IF-ELSEE 구문 사용)
DECLARE
    V_NUM NUMBER := 1;
BEGIN
    WHILE V_NUM < 10 LOOP
        IF MOD(V_NUM, 3) = 0 THEN
            DBMS_OUTPUT.PUT_LINE('출력 : ' || V_NUM);
        END IF;
        V_NUM := V_NUM + 1;
    END LOOP;
END;
/

-- Q2) 역순으로 4부터 0까지 표현하기
BEGIN
    FOR I IN REVERSE 0..4 LOOP
        DBMS_OUTPUT.PUT_LINE('현재 I의 값 : ' || I);
    END LOOP;
END;
/

-- Q3) RECORD를 이용한 데이터 삽입
-- 200번 부서, '스마트팩토리', '구미'
CREATE TABLE DEPT_RECORD2
    AS SELECT * FROM DEPT;
DESC DEPT_RECORD2;
SELECT * FROM DEPT_RECORD2;

DECLARE
    TYPE DEPTREC IS RECORD(
        deptno NUMBER(2) NOT NULL := 99,
        dname DEPT.DNAME%TYPE,
        loc DEPT.LOC%TYPE
    );
    deptrecord DEPTREC;
BEGIN
    deptrecord.deptno := 88;
    deptrecord.dname := '연구';
    deptrecord.loc := '구미';
    
INSERT INTO DEPT_RECORD2
VALUES deptrecord;
END;
/
