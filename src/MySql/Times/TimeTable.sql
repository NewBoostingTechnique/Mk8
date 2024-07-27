CREATE TABLE IF NOT EXISTS `times` (
  `Id` BINARY(16) NOT NULL,
  `CourseId` BINARY(16) NOT NULL,
  `Date` DATE NOT NULL,
  `PlayerId` BINARY(16) NOT NULL,
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
