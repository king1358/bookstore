import React, { useEffect } from "react";
import { Container, Typography, Button, Grid } from "@material-ui/core";
import { Link } from "react-router-dom";

import CartItem from "./CartItem/CartItem";
import useStyles from "./styles";
import { useState } from "react";
import { formatter } from "../../lib/formatM";
import { useQuery } from "react-query";
import jwt_decode from "jwt-decode";
import axios from "axios";

const Cart = ({ onEmptyCart }) => {
  const classes = useStyles();
  const [listBook, setListBook] = useState([]);
  const handleEmptyCart = () => onEmptyCart();
  const [total, setTotal] = useState(0);
  const [id_u, setId_u] = useState(
    sessionStorage.getItem("access_token")
      ? jwt_decode(sessionStorage.getItem("access_token")).id
      : "NoneLogin"
  );

  const handleUpdateCartQty = async (
    lineItemIdCart,
    lineItemId,
    quantity,
    total
  ) => {
    let data = {
      id_c: lineItemIdCart,
      id_b: lineItemId,
      amount: quantity,
      total: total,
      token: sessionStorage.getItem("access_token"),
    };
    axios
      .put(`https://localhost:44348/api/Cart/updateCart`, data, {
        headers: {
          "Content-Type": "application/json",
          "Access-Control-Allow-Origin": "*",
        },
      })
      .then((res) => {
        setTotal(res.data.total);
      })
      .catch((error) => console.log(error));
  };

  const onRemoveFromCart = (lineItemCart, lineItemId) => {
    // let data = {
    //   id_c: lineItemCart,
    //   id_b: lineItemId,
    // };
    // console.log(data);
    axios
      .delete(
        `https://localhost:44348/api/Cart/RemoveItemCart?id_c=${lineItemCart}&id_b=${lineItemId}&token=${sessionStorage.getItem(
          "access_token"
        )}`,
        {
          headers: {
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*",
          },
        }
      )
      .then((res) => {
        setTotal(res.data.total);
        let temp = listBook.filter((obj) => {
          return obj.id_b != lineItemId;
        });
        setListBook(temp);
        console.log("temp: ", temp);
      })
      .catch((error) => console.log(error));
  };

  const renderEmptyCart = () => (
    <Typography variant="subtitle1">
      You have no items in your shopping cart,
      <Link className={classes.link} to="/">
        {" "}
        start adding some
      </Link>
      !
    </Typography>
  );
  const [refesh, setRefesh] = useState(true);
  // if (!cart) return "Loading";
  const cart = useQuery(
    ["cart", refesh],
    async () =>
      (await axios.get(`https://localhost:44348/api/Cart?id=${id_u}`)).data
  );
  const products = useQuery(
    ["products", refesh],
    async () => (await axios.get(`https://localhost:44348/api/Book`)).data
  );
  const renderCart = () => (
    <>
      <Grid container spacing={4}>
        {listBook.map((lineItem) => (
          <Grid item xs={12} sm={4} key={lineItem.id_b}>
            <CartItem
              item={lineItem}
              onUpdateCartQty={handleUpdateCartQty}
              onRemoveFromCart={onRemoveFromCart}
            />
          </Grid>
        ))}
      </Grid>
      <div className={classes.cardDetails}>
        <Typography variant="h5">
          Subtotal: <b>{formatter.format(total)}</b>
        </Typography>
        <div>
          <Button
            className={classes.emptyButton}
            size="large"
            type="button"
            variant="contained"
            color="secondary"
            // onClick={handleEmptyCart}
          >
            Empty cart
          </Button>
          <Button
            className={classes.checkoutButton}
            component={Link}
            to="/checkoutTemp"
            size="large"
            type="button"
            variant="contained"
          >
            Checkout
          </Button>
        </div>
      </div>
    </>
  );

  useEffect(() => {
    console.log("cart", cart);
    if (cart.data && products.data) {
      console.log("propduct", products);
      let t = [];
      // console.log()
      console.log("As212", products);
      let data = cart.data;
      console.log("dataCart: ", data);
      data.cart.map((info, index) => {
        console.log(info);
        let temp = products.data.find((book) => book.id === info.id_b);
        let temp2 = {
          name: temp.name,
          source: temp.sourceimg,
          total: info.total,
          amount: info.amount,
          id_b: temp.id,
          id_c: info.id_c,
          price: temp.price,
        };
        let te = total + info.total;
        setTotal(data.total);
        t = [...t, temp2];
      });
      setListBook(t);
      // console.log(cart);
    }
    // console.log("AAAA", cart.data);
  }, [cart.data, products.data]);

  return (
    <Container>
      <div className={classes.toolbar} />
      {id_u == "NoneLogin" && (
        <Typography className={classes.title} variant="h5" gutterBottom>
          <b>Login to use this</b>
        </Typography>
      )}
      {cart.isSuccess == true && (
        <div>
          <Typography className={classes.title} variant="h5" gutterBottom>
            <b>Your Shopping Cart</b>
          </Typography>
          <hr />
          {!listBook ? renderEmptyCart() : renderCart()}
        </div>
      )}
      {cart.isError == true && (
        <Typography className={classes.title} variant="h5" gutterBottom>
          <b>Login to use this</b>
        </Typography>
      )}
    </Container>
  );
};

export default Cart;
