DROP procedure IF EXISTS `PlayerInsert`;

CREATE PROCEDURE `PlayerInsert` (
  IN CountryId BINARY(16),
  IN Id BINARY(16),
  IN ProofTypeId BINARY(16),
  IN RegionId BINARY(16)
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
