DROP procedure IF EXISTS `PersonInsert`;

CREATE PROCEDURE `PersonInsert` (
  IN PersonId VARCHAR(32),
  IN PersonName VARCHAR(255)
)
BEGIN
  INSERT INTO persons(
    Id,
    Name
  )
  VALUES(
    PersonId,
    PersonName
  );
END;
