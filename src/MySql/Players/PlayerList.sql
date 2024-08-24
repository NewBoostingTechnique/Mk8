DROP procedure IF EXISTS `PlayerList`;

CREATE PROCEDURE `PlayerList` ()
BEGIN
  SELECT
    MAX(times.Date) AS 'Active',
    countries.Name AS 'CountryName',
    persons.Name AS 'Name',
    regions.Name AS 'RegionName'
  FROM
    players
    JOIN
      countries ON players.CountryId = countries.Id
    JOIN
      persons on players.Id = persons.Id
    JOIN
      regions ON players.RegionId = regions.Id
    LEFT OUTER JOIN
      times ON players.Id = times.PlayerId
  GROUP BY
    players.Id;
END;
