DROP procedure IF EXISTS `PlayerExists`;

DELIMITER $$
CREATE PROCEDURE `PlayerExists` (
  IN PlayerName VARCHAR(255)
)
BEGIN
  SELECT
    1
  FROM
    players
  WHERE
    players.Name = PlayerName;
END$$

DELIMITER ;
