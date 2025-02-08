CREATE TABLE IF NOT EXISTS `new` (
  `Id` BINARY(16) NOT NULL,
  `Title` VARCHAR(255) NOT NULL,
  `Date` DATE NOT NULL,
  `Body` TEXT NOT NULL,
  `AuthorPersonId` BINARY(16) NOT NULL,
  PRIMARY KEY (`Id`),
  CONSTRAINT `News_Author`
    FOREIGN KEY (`AuthorPersonId`)
    REFERENCES `person` (`Id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- #region Create.

DROP PROCEDURE IF EXISTS `new_create`;

CREATE PROCEDURE `new_create` (
  IN Id BINARY(16),
  IN Title VARCHAR(255),
  IN Date DATE,
  IN Body TEXT,
  IN AuthorPersonId BINARY(16)
)
BEGIN
  INSERT INTO new(
    Id,
    Body,
    Date,
    Title,
    AuthorPersonId
  )
  VALUES(
    Id,
    Body,
    Date,
    Title,
    AuthorPersonId
  );
END;

-- #endregion Create.

-- #region Delete.

DROP PROCEDURE IF EXISTS `new_delete`;

CREATE PROCEDURE `new_delete` ()
BEGIN
  DELETE FROM new;
END;

-- #endregion Delete.

-- #region Index.

DROP PROCEDURE IF EXISTS `new_index`;

CREATE PROCEDURE `new_index` ()
BEGIN
  SELECT
    new.Title AS 'Title',
    new.Date AS 'Date',
    new.Body AS 'Body',
    person.Name AS 'AuthorName'
  FROM
    new
    JOIN
      person ON new.AuthorPersonId = person.Id
  ORDER BY
    new.Date DESC;
END;

-- #endregion Index.
