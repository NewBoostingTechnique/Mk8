DROP procedure IF EXISTS `PlayerDelete`;

DELIMITER $$
CREATE PROCEDURE `PlayerDelete` (
  IN Id VARCHAR(32)
)
BEGIN
  DELETE
  FROM    players
  WHERE   players.Id = Id;
END$$

DELIMITER ;

