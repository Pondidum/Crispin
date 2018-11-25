import React, { Component } from "react";
import { Col } from "reactstrap";

import EditHeader from "./edit-header";
import ConditionModeEditor from "./condition-mode-editor";
import Conditions from "./conditions";

class ConditionEditor extends Component {
  constructor(props) {
    super(props);
    this.state = { editing: false };
  }

  render() {
    const startEdit = () => this.setState({ editing: true });
    const cancelEdit = () => this.setState({ editing: false });

    return (
      <Col md="12">
        <EditHeader
          editing={this.state.editing}
          title="Conditions"
          startEdit={startEdit}
          cancelEdit={cancelEdit}
          acceptEdit={cancelEdit}
        />
        <ConditionModeEditor editing={this.state.editing} />
        <Conditions />
      </Col>
    );
  }
}

export default ConditionEditor;
