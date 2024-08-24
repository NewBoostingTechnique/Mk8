DROP procedure IF EXISTS `PlayerInsert`;

CREATE PROCEDURE `PlayerInsert` (
  IN CountryId BINARY(16),
  IN Id BINARY(16),
  IN RegionId BINARY(16)
)
BEGIN
  INSERT INTO players(
    CountryId,
    Id,
    RegionId
  )
  VALUES(
    CountryId,
    Id,
    RegionId
  );
END;
