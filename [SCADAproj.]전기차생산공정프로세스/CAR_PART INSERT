CREATE TABLE CAR_PART (
    차종모델 VARCHAR2(100),
    부품모델명 VARCHAR2(100),
    부품수량 NUMBER CHECK (부품수량 = 1), -- 부품수량이 1로 고정됨
    PRIMARY KEY (차종모델, 부품모델명),
    FOREIGN KEY (차종모델) REFERENCES CARNAME(차종모델),
    FOREIGN KEY (부품모델명) REFERENCES PART(부품모델명)
);
SELECT * FROM CAR_PART;

INSERT INTO CAR_PART VALUES ('테슬라 모델 X', 'Panasonic18650_Cell', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 X', 'TeslaModelX_BP', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 X', 'TeslaModelX_Front_Motor', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 X', 'TeslaModelX_Rear_Motor', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 X', 'TeslaModelX_Inverter', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 X', 'TeslaModelX_DriveUnit', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 X', 'Tesla_FSD_Computer', 1);

INSERT INTO CAR_PART VALUES ('테슬라 모델 S', 'Panasonic18650_Cell', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 S', 'TeslaModelS_BP', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 S', 'TeslaModelS_Front_Motor', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 S', 'TeslaModelS_Rear_Motor', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 S', 'TeslaModelS_Inverter', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 S', 'TeslaModelS_DriveUnit', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 S', 'Tesla_FSD_Computer', 1);

INSERT INTO CAR_PART VALUES ('테슬라 모델 3', 'Panasonic2170_Cell', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 3', 'TeslaModel3_BP', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 3', 'TeslaModel3_Front_Motor', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 3', 'TeslaModel3_Rear_PMSM', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 3', 'TeslaModel3_Inverter', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 3', 'TeslaModel3_DriveUnit', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 3', 'Tesla_FSD_Computer', 1);

INSERT INTO CAR_PART VALUES ('테슬라 모델 Y', 'Panasonic2170_Cell', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 Y', 'TeslaModelY_BP', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 Y', 'TeslaModelY_Front_Motor', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 Y', 'TeslaModelY_Rear_PMSM', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 Y', 'TeslaModelY_Inverter', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 Y', 'TeslaModelY_DriveUnit', 1);
INSERT INTO CAR_PART VALUES ('테슬라 모델 Y', 'Tesla_FSD_Computer', 1);
