DROP procedure IF EXISTS `NewInsert`;

DELIMITER $$
CREATE PROCEDURE `NewInsert` (
  IN AuthorPersonId VARCHAR(32),
  IN NewBody TEXT,
  IN NewDate DATE,
  IN NewId VARCHAR(32),
  IN NewTitle VARCHAR(255)
)
BEGIN
  INSERT INTO news(
    AuthorPersonId,
    Body,
    Date,
    Id,
    Title
  )
  VALUES(
    AuthorPersonId,
    NewBody,
    NewDate,
    NewId,
    NewTitle
  );
END$$

DELIMITER ;
