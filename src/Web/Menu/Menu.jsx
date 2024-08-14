import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import NewspaperIcon from '@mui/icons-material/Newspaper';
import PeopleIcon from '@mui/icons-material/People';
import PublishIcon from '@mui/icons-material/Publish';
import RuleIcon from '@mui/icons-material/Rule';

export default function Menu({ showProgress }) {
  return (
    <List className='menu' onClick={showProgress}>
      <ListItem key='News' disablePadding>
        <ListItemButton component={Link} to='/news/'>
          <ListItemIcon>
            <NewspaperIcon />
          </ListItemIcon>
          <ListItemText primary='News' />
        </ListItemButton>
      </ListItem>
      {/* TODO: How's this going to work?
            MK Forum?
            Discord?
            Authenticate Players? */}
      <ListItem key='Submit' disablePadding>
        <ListItemButton>
          <ListItemIcon>
            <PublishIcon />
          </ListItemIcon>
          <ListItemText primary='Submit' />
        </ListItemButton>
      </ListItem>
      <ListItem key='Rules' disablePadding>
        <ListItemButton component={Link} to='/rules/'>
          <ListItemIcon>
            <RuleIcon />
          </ListItemIcon>
          <ListItemText primary='Rules' />
        </ListItemButton>
      </ListItem>
      <ListItem key='Players' disablePadding>
        <ListItemButton component={Link} to='/players/'>
          <ListItemIcon>
            <PeopleIcon />
          </ListItemIcon>
          <ListItemText primary='Players' />
        </ListItemButton>
      </ListItem>
    </List>
  );
}

Menu.propTypes = {
  showProgress: PropTypes.func.isRequired,
};
