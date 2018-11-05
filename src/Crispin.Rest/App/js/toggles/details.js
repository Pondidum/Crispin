import React, { Component } from "react";
import { Row, Col, Button, Input } from "reactstrap";

import Conditions from "./conditions";
import ToggleGraph from "./graph";
import Glyph from "../util/glyph";

const EditHeader = ({ title, startEdit, children }) => (
  <h4 className="d-inline">
    {title}{" "}
    <a href="#" onClick={startEdit}>
      <Glyph name="pencil" />
    </a>
  </h4>
);

class Editable extends Component {
  constructor(props) {
    super(props);
    this.state = { editing: false };
  }

  viewMode() {
    const startEdit = e => {
      e.preventDefault();
      this.setState({ editing: true });
    };
    return (
      <Col md="12">
        <EditHeader title={this.props.title} startEdit={startEdit} />
        <p>{this.props.value}</p>
      </Col>
    );
  }

  editMode() {
    const cancelEdit = e => {
      e.preventDefault();
      this.setState({ editing: false });
    };

    const acceptEdit = e => {
      e.preventDefault();
      this.setState({ editing: false });
    };

    const handleKeyDown = e => {
      if (e.key == "Enter") {
        return acceptEdit(e);
      }
      if (e.key == "Escape") {
        return cancelEdit(e);
      }
    };

    return (
      <Col md="12">
        <h4 className="d-inline">{this.props.title}</h4>
        <small className="d-inline float-right">
          <a href="#" onClick={cancelEdit}>
            cancel
          </a>{" "}
          <a href="#" onClick={acceptEdit}>
            ok
          </a>
        </small>
        <Input
          type="text"
          defaultValue={this.props.value}
          onKeyDown={handleKeyDown}
          autoFocus
        />
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
        <EditHeader title="Conditions" startEdit={() => {}} />
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
