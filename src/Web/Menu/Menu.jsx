import { Link } from 'react-router-dom';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import NewspaperIcon from '@mui/icons-material/Newspaper';
import PeopleIcon from '@mui/icons-material/People';
import PublishIcon from '@mui/icons-material/Publish';

export default function Menu({ showProgress }) {
  return (
    <>
      <List className='menu' onClick={showProgress}>
        <ListItem key='News' disablePadding>
          <ListItemButton component={Link} to='/news/'>
            <ListItemIcon>
              <NewspaperIcon />
            </ListItemIcon>
            <ListItemText primary='News' />
          </ListItemButton>
        </ListItem>
        <ListItem key='Submit' disablePadding>
          <ListItemButton>
            <ListItemIcon>
              <PublishIcon />
            </ListItemIcon>
            <ListItemText primary='Submit' />
          </ListItemButton>
        </ListItem>
        <ListItem key='Players' disablePadding>
          <ListItemButton component={Link} to='/player/'>
            <ListItemIcon>
              <PeopleIcon />
            </ListItemIcon>
            <ListItemText primary='Players' />
          </ListItemButton>
        </ListItem>
      </List>
    </>
  );
}
