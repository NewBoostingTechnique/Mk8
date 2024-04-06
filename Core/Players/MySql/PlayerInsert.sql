DROP procedure IF EXISTS `PlayerInsert`;

DELIMITER $$
CREATE PROCEDURE `PlayerInsert` (
  IN CountryId VARCHAR(32),
  IN PlayerId VARCHAR(32),
  IN PlayerName VARCHAR(255),
  IN ProofTypeId VARCHAR(32),
  IN RegionId VARCHAR(32)
)
BEGIN
  INSERT INTO players(
    CountryId,
    Id,
    Name,
    ProofTypeId,
    RegionId
  )
  VALUES(
    CountryId,
    PlayerId,
    PlayerName,
    ProofTypeId,
    RegionId
  );
END$$

DELIMITER ;
