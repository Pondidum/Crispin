import React, { Component } from "react";
import { Row, Col, Button, Input } from "reactstrap";

import Conditions from "./conditions";
import ToggleGraph from "./graph";
import Glyph from "../util/glyph";

const EditHeader = ({ editing, title, startEdit, cancelEdit, acceptEdit }) => {
  const viewActions = (
    <a href="#" onClick={startEdit} className="ml-1 align-text-bottom">
      <Glyph name="pencil" />
    </a>
  );

  const editingActions = (
    <small className="d-inline float-right">
      <a href="#" onClick={cancelEdit}>
        cancel
      </a>{" "}
      <a href="#" onClick={acceptEdit}>
        ok
      </a>
    </small>
  );

  return (
    <div>
      <h4 className="d-inline">{title}</h4>
      {editing ? editingActions : viewActions}
    </div>
  );
};

class Editable extends Component {
  constructor(props) {
    super(props);
    this.state = { editing: false };
  }

  viewMode() {
    return (
      <Col md="12">
        <EditHeader editing={this.state.editing} title={this.props.title} />
      </Col>
    );
  }

  render() {
    const startEdit = e => {
      e.preventDefault();
      this.setState({ editing: true });
    };

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

    const editor = (
      <Input
        type="text"
        defaultValue={this.props.value}
        onKeyDown={handleKeyDown}
        autoFocus
      />
    );
    const viewer = <p>{this.props.value}</p>;

    return (
      <Col md="12">
        <EditHeader
          editing={this.state.editing}
          title={this.props.title}
          startEdit={startEdit}
          cancelEdit={cancelEdit}
          acceptEdit={acceptEdit}
        />
        {this.state.editing ? editor : viewer}
      </Col>
    );
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
