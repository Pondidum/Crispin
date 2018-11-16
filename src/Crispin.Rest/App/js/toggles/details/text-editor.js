import React, { Component } from "react";
import { Col, Input } from "reactstrap";

import EditHeader from "./edit-header";

class TextEditor extends Component {
  constructor(props) {
    super(props);
    this.state = { editing: false };
    this.editor = React.createRef();
  }

  render() {
    const startEdit = () => this.setState({ editing: true });
    const cancelEdit = () => this.setState({ editing: false });
    const acceptEdit = () => {
      this.setState({ editing: false });
      this.props.onAcceptEdit(this.editor.current.value);
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
        innerRef={this.editor}
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

export default TextEditor;
