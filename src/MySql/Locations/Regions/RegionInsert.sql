
DROP procedure IF EXISTS `RegionInsert`;

CREATE PROCEDURE `RegionInsert` (
  IN Id BINARY(16),  
  IN Name VARCHAR(255),
  IN CountryId BINARY(16)
)
BEGIN
  INSERT INTO regions(
    Id,
    Name,
    CountryId
  )
  VALUES(
    Id,
    Name,
    CountryId
  );
END;
