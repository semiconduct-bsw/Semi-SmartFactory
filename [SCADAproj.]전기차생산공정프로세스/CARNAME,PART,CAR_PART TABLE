CREATE TABLE PART(
부품모델명 VARCHAR2(40) NOT NULL,
부품수량 NUMBER(20) NOT NULL);

CREATE TABLE CARNAME(
차종모델 VARCHAR2(20) NOT NULL,
차량수량 NUMBER(20) NOT NULL);

CREATE TABLE CAR_PART (
    차종모델 VARCHAR2(100),
    부품모델명 VARCHAR2(100),
    부품수량 NUMBER CHECK (부품수량 = 1), -- 부품수량이 1로 고정됨
    PRIMARY KEY (차종모델, 부품모델명),
    FOREIGN KEY (차종모델) REFERENCES CAR_MODEL(차종모델),
    FOREIGN KEY (부품모델명) REFERENCES PART(부품모델명)
);

-- CAR_PART 데이터 삽입
  -- 'CAR A' 모델에 대해 7개의 부품 추가
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR A', 'PART1', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR A', 'PART2', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR A', 'PART3', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR A', 'PART4', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR A', 'PART5', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR A', 'PART6', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR A', 'PART7', 1);

-- 'CAR B' 모델에 대해 7개의 부품 추가
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR B', 'PART1', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR B', 'PART2', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR B', 'PART3', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR B', 'PART4', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR B', 'PART5', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR B', 'PART6', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR B', 'PART7', 1);

-- 'CAR C' 모델에 대해 7개의 부품 추가
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR C', 'PART1', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR C', 'PART2', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR C', 'PART3', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR C', 'PART4', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR C', 'PART5', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR C', 'PART6', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR C', 'PART7', 1);

-- 'CAR D' 모델에 대해 7개의 부품 추가
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR D', 'PART1', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR D', 'PART2', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR D', 'PART3', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR D', 'PART4', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR D', 'PART5', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR D', 'PART6', 1);
INSERT INTO CAR_PART (차종모델, 부품모델명, 부품수량) VALUES ('CAR D', 'PART7', 1);
