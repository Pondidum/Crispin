import React from "react";
import { Col, Row } from "react-bootstrap";
import { NavLink } from "react-router-dom";
import CreateToggle from "../CreateToggle";

export default ({ children }) => (
  <div className="container-fluid" id="root">
    <Row>
      <Col sm={3} md={2} className="sidebar">
        <h1>Crispin</h1>
        <ul className="list-unstyled">
          <li>
            <NavLink exact to={"/"} activeClassName="active">
              <span className="glyphicon glyphicon-home" /> Home
            </NavLink>
          </li>
          <li>
            <CreateToggle />
          </li>
        </ul>
      </Col>
      <Col sm={9} md={10} smOffset={3} mdOffset={2} className="main">
        {children}
      </Col>
    </Row>
  </div>
);
