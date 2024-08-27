-- 실습 6-13 LAPD, RPAD 함수 활용 출력
SELECT 'Oracle',
    LPAD('Oracle', 10, '#') AS LPAD_1,
    RPAD('Oracle', 10, '*') AS RPAD_1,
    LPAD('Oracle', 10) AS LPAD_2,
    LPAD('Oracle', 10) AS RPAD_2 FROM DUAL;
    
-- 실습 6-14 RPAD 함수 활용 주민 뒷자리 *표시 출력
SELECT
    RPAD('971225-', 14, '*') AS RPAD_JMMO,
    RPAD('010-1234-', 13, '*') AS RPAD_PHONE FROM DUAL;

-- 실습 6-15 두 열 사이 콜론 넣고 연결
SELECT CONCAT(EMPNO, ENAME),
       CONCAT(EMPNO, CONCAT(' : ', ENAME)) FROM EMP;
       
-- 실습 6-16 TRIM 함수로 공백 제거하여 출력
SELECT '[' || TRIM(' _ _ORACLE_ _ ') || ']' AS TRIM,
       '[' || TRIM(LEADING FROM ' _ _ORACLE_ _ ') || ']' AS TRIM_LEADING,
       '[' || TRIM(TRAILING FROM ' _ _ORACLE_ _ ') || ']' AS TRIM_TRAILING,
       '[' || TRIM(BOTH FROM ' _ _ORACLE_ _ ') || ']' AS TRIM_BOTH
FROM DUAL;

-- 실습 6-17 TRIM 함수로 삭제할 문자 _ 삭제 후 출력하기
SELECT '[' || TRIM('_' FROM '_ _ORACLE_ _') || ']' AS TRIM,
       '[' || TRIM(LEADING '_' FROM '_ _ORACLE_ _') || ']' AS TRIM_LEADING,
       '[' || TRIM(TRAILING '_' FROM '_ _ORACLE_ _') || ']' AS TRIM_TRAILING,
       '[' || TRIM(BOTH '_' FROM '_ _ORACLE_ _') || ']' AS TRIM_BOTH
FROM DUAL;

-- 실습 6-18 TRIM, LTRIM, RTRIM 활용
SELECT '[' || TRIM(' _ORACLE_ ') || ']' AS TRIM,
       '[' || LTRIM(' _ORACLE_ ') || ']' AS LTRIM,
       '[' || LTRIM('<_ORACLE_>', '_<') || ']' AS LTRIM_2,
       '[' || RTRIM(' _ORACLE_ ') || ']' AS RTRIM,
       '[' || RTRIM('<_ORACLE_>', '>_') || ']' AS RTRIM_2
FROM DUAL;
