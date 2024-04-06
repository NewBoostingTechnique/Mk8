DROP procedure IF EXISTS `PlayerList`;

DELIMITER $$
CREATE PROCEDURE `PlayerList` ()
BEGIN
  SELECT
    MAX(times.Date) AS 'Active',
    countries.Name AS 'CountryName',
    players.Name as 'Name',
    prooftypes.Description AS 'ProofTypeDescription',
    regions.Name AS 'RegionName'
  FROM
    players
    JOIN
      countries ON players.CountryId = countries.Id
    JOIN
      prooftypes ON players.ProofTypeId = prooftypes.Id
    JOIN
      regions ON players.RegionId = regions.Id
    LEFT OUTER JOIN
      times ON players.Id = times.PlayerId
  GROUP BY
    players.Id;
END$$

DELIMITER ;