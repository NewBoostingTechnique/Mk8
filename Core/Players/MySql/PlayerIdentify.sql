DROP procedure IF EXISTS `PlayerIdentify`;

DELIMITER $$
CREATE PROCEDURE `PlayerIdentify` (
  IN Name VARCHAR(255)
)
BEGIN
  SELECT
    players.Id
  FROM
    players
    JOIN
      persons ON players.Id = persons.Id
  WHERE
    persons.Name = Name;
END$$

DELIMITER ;
