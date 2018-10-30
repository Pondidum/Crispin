import React from "react";
import { Card, CardBody, CardTitle, CardImg } from "reactstrap";

const Graph = ({ title }) => (
  <Card className="mb-3">
    <CardBody>
      <CardTitle>{title}</CardTitle>
    </CardBody>
    <CardImg
      width="100%"
      src="https://placeholdit.imgix.net/~text?txtsize=33&txt=318%C3%97100&w=318&h=100"
      alt="Card image cap"
    />
  </Card>
);

export default Graph;
