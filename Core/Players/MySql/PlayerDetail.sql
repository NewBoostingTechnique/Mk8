DROP procedure IF EXISTS `PlayerDetail`;

DELIMITER $$
CREATE PROCEDURE `PlayerDetail` (
  IN PlayerId VARCHAR(32)
)
BEGIN
  SELECT
    MAX(times.Date) AS 'Active',
    countries.Name AS 'CountryName',
    players.Name,
    prooftypes.Description AS 'ProofTypeDescription',
    regions.Name AS 'RegionName'
  FROM
    players
    JOIN
      prooftypes ON players.ProofTypeId = prooftypes.Id
    JOIN
      countries ON players.CountryId = countries.Id
    JOIN
      regions ON players.RegionId = regions.Id
    LEFT OUTER JOIN
      times ON players.Id = times.PlayerId
  WHERE
    players.Id = PlayerId
  GROUP BY
    players.Id;

  SELECT
    courses.Name AS 'CourseName',
    times.TimeSpan
  FROM
    courses
    LEFT OUTER JOIN
      times ON courses.Id = times.CourseId
  WHERE
    times.playerId IS NULL
    OR
      times.playerId = PlayerId;

END$$

DELIMITER ;
