import React from 'react';
import { Container, Typography, Button, Grid } from '@material-ui/core';
import { Link } from 'react-router-dom';

import CartItem from './CartItem/CartItem';
import useStyles from './styles';
import { useEffect } from 'react';
import jwt_decode from "jwt-decode";
import Cookies from 'universal-cookie';
import axios from "axios";
import { useState } from 'react';

const Cart = ({onUpdateCartQty, onRemoveFromCart, onEmptyCart }) => {
  const [cart,setCart] = useState([])
  const classes = useStyles();
  const cookies = new Cookies();
  const token = cookies.get('access_token')
  const [total,setTotal] = useState(0)
  let decoded = jwt_decode(token);

  const getCart = async () =>{
    axios.get(`http://localhost:30766/api/Cart?username=${decoded.username}`, {
      headers: {
        "Content-Type": "application/json",
        "Access-Control-Allow-Origin": "*"
      }
    })
      .then(res => {
        // setProduct(res.data)
        setCart(res.data.items)
        console.log(res.data)
      })
      .catch(error => console.log(error));
  }

  useEffect(() => {
    getCart();
    // fetchProducts();
    // fetchCart();
  }, []);

  // useEffect(() => {
  //   getCart();
  //   // fetchProducts();
  //   // fetchCart();
  // }, [cart]);

  const handleEmptyCart = () => onEmptyCart();

  const removeItemCart = (itemId)=>{
      let newCart = cart.filter(item => item.id_book != itemId)
      console.log("^^^^^^")
      console.log(newCart)
      console.log("^^^^^^")
      setCart(newCart)
  }

  const renderEmptyCart = () => (
    <Typography variant="subtitle1">You have no items in your shopping cart,
      <Link className={classes.link} to="/"> start adding some</Link>!
    </Typography>
  );

  if (!cart) return 'Loading';
  
  

  const renderCart = () => (
    
    <>
    {/* <p>AAAAAAAAAA</p> */}
      <Grid container spacing={4}>
        {cart.map((lineItem) => (
          <Grid item xs={12} sm={4} key={lineItem.id}>
            <CartItem item={lineItem} onUpdateCartQty={onUpdateCartQty} onRemoveFromCart={onRemoveFromCart} onRemoveItem = {removeItemCart} calTotal = {setTotal} total ={total} />
          </Grid>
        ))}
      </Grid>
      <div className={classes.cardDetails}>
      {/* <Typography variant="h5" >Subtotal: <b >{total}</b></Typography> */}
        <div>
          <Button className={classes.emptyButton} size="large" type="button" variant="contained" color="secondary" onClick={handleEmptyCart}>Empty cart</Button>
          <Button className={classes.checkoutButton} component={Link} to="/checkout" size="large" type="button" variant="contained" >Checkout</Button>
        </div>
      </div>
    </>
  );

  return (
    <Container>
      <div className={classes.toolbar} />
      <Typography className={classes.title} variant="h5" gutterBottom><b>Your Shopping Cart</b></Typography>
      <hr/>
      { !cart.length ? renderEmptyCart() : renderCart() }
    </Container>
  );
};

export default Cart;
