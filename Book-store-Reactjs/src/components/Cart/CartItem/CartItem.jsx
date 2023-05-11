import React, { useState } from 'react';
import { Typography, Button, Card, CardActions, CardContent, CardMedia } from '@material-ui/core';

import useStyles from './styles';

import { useEffect } from 'react';
import axios from "axios";

const CartItem = ({ item, onUpdateCartQty, onRemoveFromCart, onRemoveItem,calTotal,total }) => {
  const classes = useStyles();
  const [product, setProduct] = useState([])
  const [click, setclick] = useState(true)

  const fetchProduct = async (id) => {
    console.log("ID: ", id)
    axios.get(`http://localhost:30766/api/Book/id?id=${id}`, {
      headers: {
        "Content-Type": "application/json",
        "Access-Control-Allow-Origin": "*"
      }
    })
      .then(res => {
        let temp = total + res.data.price * item.quantity
        console.log("temp: ",temp)
        calTotal(temp);
        setProduct(res.data)
        // console.log(res.data)
      })
      .catch(error => console.log(error));
    // console.log({ response });
    // const { name, price, media, quantity, description } = response;
  };

  // useEffect(() => {
  //   console.log(item.id_book)
  //   console.log("AAAAA")
  //   fetchProduct(item.id_book)
  //   // fetchProducts();
  //   // fetchCart();
  // }, []);
  useEffect(() => {
    fetchProduct(item.id_book)
    
  }, [item]);
  const handleUpdateCartQty = (lineItemId, newQuantity) => {
    // console.log(123)
    if (newQuantity == 0) {
      onRemoveItem(lineItemId)
      onUpdateCartQty(lineItemId, newQuantity)
    }
    else {
      onUpdateCartQty(lineItemId, newQuantity)
      fetchProduct(item.id_book)
    }
  };

  const handleRemoveFromCart = (lineItemId) => onRemoveFromCart(lineItemId);

  return (
    <Card className="cart-item">
      <CardMedia image={product.sourceimg} alt={item.nameb} className={classes.media} />
      <CardContent className={classes.cardContent}>
        <Typography variant="h6">{product.nameb}</Typography>
        <Typography variant="h6" color='secondary' >{product.price * item.quantity}</Typography>
      </CardContent>
      <CardActions className={classes.cardActions}>
        <div className={classes.buttons}>
          <Button type="button" size="small" onClick={() => {
            handleUpdateCartQty(product.id, item.quantity - 1)
            item.quantity = item.quantity - 1
          }}>-</Button>
          {/* <Typography>&nbsp;{product.quantity}&nbsp;</Typography> */}
          <Typography>&nbsp;{item.quantity}&nbsp;</Typography>

          <Button type="button" size="small" onClick={() => {
            handleUpdateCartQty(product.id, item.quantity + 1)
            item.quantity = item.quantity + 1
          }}>+</Button>
        </div>
        <Button className={classes.button} variant="contained" type="button" color='secondary' onClick={() => handleRemoveFromCart(product.id)}>Remove</Button>
      </CardActions>
    </Card>
  );
};

export default CartItem;
