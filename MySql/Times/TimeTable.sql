CREATE TABLE IF NOT EXISTS `times` (
  `Id` VARCHAR(32) NOT NULL,
  `CourseId` VARCHAR(32) NOT NULL,
  `Date` DATE NOT NULL,
  `PlayerId` VARCHAR(32) NOT NULL,
  `TimeSpan` TIME(3) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `Player_idx` (`PlayerId` ASC) VISIBLE,
  CONSTRAINT `Time_Course`
    FOREIGN KEY (`CourseId`)
    REFERENCES `courses` (`Id`),
  CONSTRAINT `Time_Player`
    FOREIGN KEY (`PlayerId`)
    REFERENCES `players` (`Id`),
  CONSTRAINT `Time_Duplicate`
    UNIQUE (`PlayerId`, `CourseId`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;
