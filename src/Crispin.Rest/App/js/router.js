import React from "react";
import { BrowserRouter as Router, Route } from "react-router-dom";
import { Container, Col, Row } from "reactstrap";

import Navigation from "./navigation";
import Dashboard from "./dashboard";
import Toggles from "./toggles";

const Users = () => <h2>Users</h2>;

const AppRouter = () => (
  <Router>
    <Container fluid={true}>
      <Row>
        <Col sm="12">
          <Navigation />
        </Col>
      </Row>
      <Row>
        <Col sm="12">
          <Route path="/" exact component={Dashboard} />
          <Route path="/toggles/" component={Toggles} />
          <Route path="/users/" component={Users} />
        </Col>
      </Row>
    </Container>
  </Router>
);

export default AppRouter;
