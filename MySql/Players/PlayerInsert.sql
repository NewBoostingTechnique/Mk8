DROP procedure IF EXISTS `PlayerInsert`;

CREATE PROCEDURE `PlayerInsert` (
  IN CountryId VARCHAR(32),
  IN Id VARCHAR(32),
  IN ProofTypeId VARCHAR(32),
  IN RegionId VARCHAR(32)
)
BEGIN
  INSERT INTO players(
    CountryId,
    Id,
    ProofTypeId,
    RegionId
  )
  VALUES(
    CountryId,
    Id,
    ProofTypeId,
    RegionId
  );
END;
