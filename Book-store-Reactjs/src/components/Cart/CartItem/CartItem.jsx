import React, { useEffect, useState } from "react";
import {
  Typography,
  Button,
  Card,
  CardActions,
  CardContent,
  CardMedia,
} from "@material-ui/core";

import useStyles from "./styles";
import { formatter } from "../../../lib/formatM";
const CartItem = ({ item, onUpdateCartQty, onRemoveFromCart }) => {
  const classes = useStyles();
  const [amount, setAmount] = useState(item.amount);
  const [total, setTotal] = useState(item.total);
  const handleUpdateCartQty = (lineItemCart, lineItemId, newQuantity) => {
    if (newQuantity != 0) {
      setTotal(newQuantity * item.price);
      onUpdateCartQty(
        lineItemCart,
        lineItemId,
        newQuantity,
        newQuantity * item.price
      );
    } else onRemoveFromCart(lineItemCart, lineItemId);
  };
  const handleRemoveFromCart = (lineItemCart, lineItemId) =>
    onRemoveFromCart(lineItemCart, lineItemId);

  useEffect(() => {}, []);

  return (
    <Card className="cart-item">
      <CardMedia
        image={item.source}
        alt={item.name}
        className={classes.media}
      />
      <CardContent className={classes.cardContent}>
        <Typography variant="h6">{item.name}</Typography>
        <Typography variant="h6" color="secondary">
          {formatter.format(total)}
        </Typography>
      </CardContent>
      <CardActions className={classes.cardActions}>
        <div className={classes.buttons}>
          <Button
            type="button"
            size="small"
            onClick={() => {
              handleUpdateCartQty(item.id_c, item.id_b, item.amount - 1);
              setAmount(item.amount - 1);
              item.amount = item.amount - 1;
            }}
          >
            -
          </Button>
          <Typography>&nbsp;{amount}&nbsp;</Typography>
          <Button
            type="button"
            size="small"
            onClick={() => {
              handleUpdateCartQty(item.id_c, item.id_b, item.amount + 1);
              setAmount(item.amount + 1);
              item.amount = item.amount + 1;
            }}
          >
            +
          </Button>
        </div>
        <Button
          className={classes.button}
          variant="contained"
          type="button"
          color="secondary"
          onClick={() => handleRemoveFromCart(item.id_c, item.id_b)}
        >
          Remove
        </Button>
      </CardActions>
    </Card>
  );
};

export default CartItem;
