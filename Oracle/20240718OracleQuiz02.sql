-- P212 Q1
SELECT DEPTNO,
    ROUND(AVG(SAL),0) AS AVG_SAL, MAX(SAL) AS MAX_SAL, MIN(SAL) AS MIN_SAL,
    COUNT (*) FROM EMP
GROUP BY DEPTNO;

-- P212 Q2
SELECT JOB, COUNT (*) AS CNT FROM EMP
GROUP BY JOB HAVING COUNT (*) >= 3;

-- P212 Q3
SELECT TO_CHAR(HIREDATE, 'YYYY') AS HIRE_YEAR,
    DEPTNO, COUNT (*) FROM EMP
GROUP BY TO_CHAR(HIREDATE, 'YYYY'), DEPTNO;