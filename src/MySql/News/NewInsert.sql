DROP procedure IF EXISTS `NewInsert`;

CREATE PROCEDURE `NewInsert` (
  IN AuthorPersonId BINARY(16),
  IN NewBody TEXT,
  IN NewDate DATE,
  IN NewId BINARY(16),
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
END;
