CREATE TABLE IF NOT EXISTS `course` (
  `Id` BINARY(16) NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Name_UNIQUE` (`Name` ASC) VISIBLE,
  INDEX `Name` (`Name` ASC) VISIBLE
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- #region Create.

DROP PROCEDURE IF EXISTS `course_create`;

CREATE PROCEDURE `course_create`(
  IN Id BINARY(16),
  IN Name VARCHAR(255)
)
BEGIN
  INSERT INTO course(
    Id,
    Name
  )
  VALUES(
    Id,
    Name
  );
END;

-- #endregion Create.

-- #region Exists.

DROP PROCEDURE IF EXISTS `course_exists`;

CREATE PROCEDURE `course_exists`(
  IN Name VARCHAR(255)
)
BEGIN
  SELECT  1
  FROM    course
  WHERE   Name = Name;
END;

-- #endregion Exists.

-- #region Identify.

DROP PROCEDURE IF EXISTS `course_identify`;

CREATE PROCEDURE `course_identify` (
  IN Name VARCHAR(255)
)
BEGIN
  SELECT
    course.Id
  FROM
    course
  WHERE
    course.Name = Name;
END;

-- #endregion Identify.

-- #region Index.

DROP PROCEDURE IF EXISTS `course_index`;

CREATE PROCEDURE `course_index`(
)
BEGIN
  SELECT  course.Name
  FROM    course;
END;

-- #endregion Index.
