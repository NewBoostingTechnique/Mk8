DROP procedure IF EXISTS `CountryInsert`;

CREATE PROCEDURE `CountryInsert` (
  IN Id BINARY(16),
  IN Name VARCHAR(255)
)
BEGIN
  INSERT INTO countries(
    Id,
    Name
  )
  VALUES(
    Id,
    Name
  );
END;
