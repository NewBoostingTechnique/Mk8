DROP procedure IF EXISTS `PlayerIdentify`;

DELIMITER $$
CREATE PROCEDURE `PlayerIdentify` (
  IN PlayerName VARCHAR(255)
)
BEGIN
  SELECT
    players.Id
  FROM
    players
  WHERE
    players.Name = PlayerName;
END$$

DELIMITER ;
