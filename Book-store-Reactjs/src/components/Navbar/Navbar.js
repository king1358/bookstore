import React from "react";
import {
  AppBar,
  Toolbar,
  IconButton,
  Badge,
  Typography,
} from "@material-ui/core";
import { ShoppingCart } from "@material-ui/icons";
import { AccountCircle } from "@material-ui/icons";
import { Link, useLocation } from "react-router-dom";
import logo from "../../assets/circles.png";
import useStyles from "./styles";
import jwt_decode from "jwt-decode";
import { useEffect } from "react";
import { useState } from "react";
const Navbar = ({ totalItems }) => {
  const classes = useStyles();
  const location = useLocation();
  const [decode, setDecode] = useState({ username: "", fullname: "" });

  useEffect(() => {
    const token = sessionStorage.getItem("access_token");
    if (token != null) setDecode(jwt_decode(token));
    else decode.username = "";
  }, []);

  return (
    <div>
      <AppBar position="fixed" className={classes.appBar} color="inherit">
        <Toolbar>
          <Typography
            component={Link}
            to="/"
            variant="h5"
            className={classes.title}
            color="inherit"
          >
            <img
              src={logo}
              alt="Book Store App"
              height="50px"
              className={classes.image}
            />
            <strong>BooK-IT</strong>
          </Typography>

          <div className={classes.grow} />
          {/* {location.pathname === '/' && ( */}
          <div className={classes.button}>
            <IconButton
              component={Link}
              to={`/cart/${decode.username}`}
              aria-label="Show cart items"
              color="inherit"
            >
              <Badge badgeContent={totalItems} color="secondary">
                <ShoppingCart />
              </Badge>
            </IconButton>
            {!sessionStorage.getItem("access_token") ? (
              <IconButton
                component={Link}
                to="/login"
                aria-label="Show cart items"
                color="inherit"
              >
                <Badge badgeContent={totalItems} color="secondary">
                  <AccountCircle />
                </Badge>
              </IconButton>
            ) : (
              <IconButton
                component={Link}
                to="/"
                aria-label="Show cart items"
                color="inherit"
              >
                <Badge badgeContent={totalItems} color="secondary">
                  <AccountCircle />
                </Badge>
              </IconButton>
            )}
          </div>
        </Toolbar>
      </AppBar>
    </div>
  );
};

export default Navbar;
