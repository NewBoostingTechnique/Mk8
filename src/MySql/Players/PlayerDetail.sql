DROP procedure IF EXISTS `PlayerDetail`;

CREATE PROCEDURE `PlayerDetail` (
  IN Id BINARY(16)
)
BEGIN
  SELECT
    MAX(times.Date) AS 'Active',
    countries.Name AS 'CountryName',
    persons.Name,
    prooftypes.Description AS 'ProofTypeDescription',
    regions.Name AS 'RegionName'
  FROM
    players
    JOIN
      countries ON players.CountryId = countries.Id
    JOIN
      persons ON players.Id = persons.Id
    JOIN
      prooftypes ON players.ProofTypeId = prooftypes.Id
    JOIN
      regions ON players.RegionId = regions.Id
    LEFT OUTER JOIN
      times ON players.Id = times.PlayerId
  WHERE
    players.Id = Id
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
    times.PlayerId IS NULL
    OR
      times.PlayerId = Id;
END;
