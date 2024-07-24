-- 실습 17-1 레코드 정의 및 사용
DECLARE
    TYPE REC_DEPT IS RECORD(
        deptno NUMBER(2) NOT NULL := 99,
        dname DEPT.DNAME%TYPE,
        loc DEPT.LOC%TYPE
    );
    dept_rec REC_DEPT;
BEGIN
    dept_rec.deptno := 99;
    dept_rec.dname := 'DATABASE';
    dept_rec.loc := 'SEOUL';
    DBMS_OUTPUT.PUT_LINE(dept_rec.deptno);
    DBMS_OUTPUT.PUT_LINE(dept_rec.dname);
    DBMS_OUTPUT.PUT_LINE(dept_rec.loc);
END;
/

-- 실습 17-2 DEPT_RECORD
CREATE TABLE DEPT_RECORD
    AS SELECT * FROM DEPT;
SELECT * FROM DEPT_RECORD;

-- 실습 17-3 레코드를 사용한 INSERT
DECLARE
    TYPE REC_DEPT IS RECORD(
        deptno NUMBER(2) NOT NULL := 99,
        dname DEPT.DNAME%TYPE,
        loc DEPT.LOC%TYPE
    );
    dept_rec REC_DEPT;
BEGIN
    dept_rec.deptno := 99;
    dept_rec.dname := 'DATABASE';
    dept_rec.loc := 'SEOUL';
INSERT INTO DEPT_RECORD
VALUES dept_rec;
END;
/

-- 실습 17-4 RECORD를 사용하여 UPDATE 해보기
-- 부서 테이블 99번 부서 데이터를 객체 값으로 수정
DECLARE
    TYPE REC_DEPT IS RECORD(
        deptno NUMBER(2) NOT NULL := 99,
        dname DEPT.DNAME%TYPE,
        loc DEPT.LOC%TYPE
    );
    dept_rec REC_DEPT;
BEGIN
    dept_rec.deptno := 50;
    dept_rec.dname := 'DB';
    dept_rec.loc := 'ANDONG';
    
    UPDATE DEPT_RECORD
    SET ROW = dept_rec
    WHERE DEPTNO = 99;
END;
/
SELECT * FROM DEPT_RECORD;
