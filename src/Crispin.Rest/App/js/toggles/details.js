import React, { Component } from "react";
import { Row, Col, Button, Input } from "reactstrap";

import Conditions from "./conditions";
import ToggleGraph from "./graph";

class Editable extends Component {
  constructor({ title, value }) {
    super();
    this.title = title;
    this.value = value;
    this.state = { editing: false };
  }

  viewMode() {
    const startEdit = e => {
      e.preventDefault();
      this.setState({ editing: true });
    };
    return (
      <Col md="12">
        <h4 className="d-inline">{this.title}</h4>
        <small className="d-inline">
          <a href="#" onClick={startEdit}>
            edit
          </a>
        </small>

        <p>{this.value}</p>
      </Col>
    );
  }

  editMode() {
    return (
      <Col md="12">
        <h4 className="d-inline">{this.title}</h4>
        <Input type="text" defaultValue={this.value} />
      </Col>
    );
  }
  render() {
    return this.state.editing ? this.editMode() : this.viewMode();
  }
}

const Details = ({ match }) => (
  <Row>
    <Col md="6">
      <Editable title="Name" value={`Toggle ${match.params.id}`} />
      <Editable
        title="Description"
        value="Does something very interesting and potentially has quite a long
          description"
      />
      <Col md="12">
        <h4>Conditions</h4>
        <Conditions />
      </Col>
    </Col>
    <Col md="6">
      <ToggleGraph title="Usage Graph" />
      <ToggleGraph title="Activation Graph" />
      <ToggleGraph title="Condition Activation Graph" />
    </Col>
  </Row>
);

export default Details;
