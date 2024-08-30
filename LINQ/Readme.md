LINQ 수업 내용 정리

-- 0. 21c 설정
SQL> alter session set "_ORACLE_SCRIPT" = true;
세션이 변경되었습니다.

SQL> commit;
커밋이 완료되었습니다.

-- 1. 계정 생성
CREATE USER smart IDENTIFIED BY factory;

-- 2. 기본 권한 부여
GRANT CONNECT, RESOURCE TO smart;

-- 무제한 개인 테이블 공간 부여하려면!!
grant unlimited tablespace to smart;

-- 임시공간 부여
alter user smart temporary tablespace temp;

-- 오라클의 경우 받아야 되는 모듈

Oracle.EntityFrameworkCore
Oracle.ManagedDataAccess.Core
-- Codefirst 등 작업시
Microsoft.EntityFramework.Tools
-- TagHelper의 Model 관련 Form 디자인할때
Microsoft.VisualStudio.Web.CodeGeneration.Design
