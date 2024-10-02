CREATE TABLE IF NOT EXISTS `time` (
  `Id` BINARY(16) NOT NULL,
  `Span` TIME(3) NOT NULL,
  `Date` DATE NOT NULL,
  `CourseId` BINARY(16) NOT NULL,
  `PlayerId` BINARY(16) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `Player_idx` (`PlayerId` ASC) VISIBLE,
  CONSTRAINT `Time_Course`
    FOREIGN KEY (`CourseId`)
    REFERENCES `course` (`Id`),
  CONSTRAINT `Time_Player`
    FOREIGN KEY (`PlayerId`)
    REFERENCES `player` (`Id`)
    ON DELETE CASCADE,
  CONSTRAINT `Time_Duplicate`
    UNIQUE (`PlayerId`, `CourseId`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- #region Create.

DROP PROCEDURE IF EXISTS `time_create`;

CREATE PROCEDURE `time_create` (
  IN Id BINARY(16),
  IN Span TIME(3),
  In Date DATE,
  IN CourseId BINARY(16),
  IN PlayerId BINARY(16)
)
BEGIN
  INSERT INTO time(
    Id,
    Span,
    Date,
    CourseId,
    PlayerId
  )
  VALUES(
    Id,
    Span,
    Date,
    CourseId,
    PlayerId
  );
END;

-- #endregion Create.

-- #region Exists.

DROP PROCEDURE IF EXISTS `time_exists`;

CREATE PROCEDURE `time_exists` (
  IN CourseId BINARY(16),
  IN PlayerId BINARY(16)
)
BEGIN
  SELECT
    1
  FROM
    time
  WHERE
    time.CourseId = CourseId
    AND
    time.PlayerId = PlayerId;
END;

-- #endregion Exists.
