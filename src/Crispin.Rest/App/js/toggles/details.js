import React from "react";
import { Row, Col, Card, CardBody, CardTitle, CardImg } from "reactstrap";

const headerWidth = 3;
const detailsWidth = 12 - headerWidth;

const Details = ({ match }) => (
  <Row>
    <Col md="5">
      <Row>
        <Col md={headerWidth}>Name</Col>
        <Col md={detailsWidth}>Some Feature Toggle {match.params.id}</Col>
      </Row>
      <Row>
        <Col md={headerWidth}>Description</Col>
        <Col md={detailsWidth}>
          does something very interesting and potentially has quite a long
          description
        </Col>
      </Row>
      <Row>
        <Col md={headerWidth}>Conditions</Col>
        <Col md={detailsWidth}>
          Toggle is active when <b>any</b> | <a href="#">all</a> conditions are
          true.
        </Col>
        <Col md={{ size: detailsWidth, offset: headerWidth }}>
          <ul>
            <li>
              When user is in group <b>Alpha Testers</b>
            </li>
            <li>
              When user is in group <b>Beta Testers</b>
            </li>
            <li>
              When All are True
              <ul>
                <li>
                  When user is in group <b>Canary</b>
                </li>
                <li>
                  When user is in <b>5%</b> of users
                </li>
              </ul>
            </li>
          </ul>
        </Col>
      </Row>
    </Col>
    <Col md="7">
      <Card>
        <CardBody>
          <CardTitle>Usage Graph</CardTitle>
        </CardBody>
        <CardImg
          width="100%"
          src="https://placeholdit.imgix.net/~text?txtsize=33&txt=318%C3%97100&w=318&h=100"
          alt="Card image cap"
        />
      </Card>
      <Card>
        <CardBody>
          <CardTitle>Activation Graph</CardTitle>
        </CardBody>
      </Card>
      <Card>
        <CardBody>
          <CardTitle>Condition Activation Graph</CardTitle>
        </CardBody>
      </Card>
    </Col>
  </Row>
);

export default Details;
