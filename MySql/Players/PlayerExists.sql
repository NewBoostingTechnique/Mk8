DROP procedure IF EXISTS `PlayerExists`;

CREATE PROCEDURE `PlayerExists` (
  IN Name VARCHAR(255)
)
BEGIN
  SELECT
    1
  FROM
    players
    JOIN
      persons ON players.Id = persons.Id
  WHERE
    persons.Name = Name;
END;
