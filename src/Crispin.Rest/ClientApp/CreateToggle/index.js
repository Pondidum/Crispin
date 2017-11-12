import React, { Component } from "react";
import { Alert, Button, Modal } from "react-bootstrap";
import { FormGroup, ControlLabel, FormControl } from "react-bootstrap";
import { connect } from "react-redux";
import { createToggle } from "./actions";

const mapStateToProps = (state, ownProps) => {
  return {
    ...ownProps,
    create: state.create
  };
};

const mapDispatchToProps = dispatch => {
  return {
    createToggle: (name, description, success, failure) =>
      dispatch(createToggle(name, description, success, failure))
  };
};

class CreateToggle extends Component {
  constructor() {
    super();
    this.renderMessage = this.renderMessage.bind(this);
    this.state = {
      showModal: false,
      name: "",
      description: "",
      failureMessages: []
    };
  }

  renderMessage() {
    const messages = this.state.failureMessages;

    if (messages.length === 0) return null;

    return (
      <Alert bsStyle="danger">
        <ul className="list-unstyled">
          {messages.map((m, i) => <li key={i}>{m}</li>)}
        </ul>
      </Alert>
    );
  }

  render() {
    const open = () => this.setState({ showModal: true });
    const close = () => this.setState({ showModal: false });

    const save = () =>
      this.props.createToggle(
        this.state.name,
        this.state.description,
        close,
        body => this.setState({ failureMessages: body.messages })
      );

    const toggleModal = e => {
      e.preventDefault();
      open();
    };

    return (
      <span>
        <a href="#" onClick={toggleModal}>
          Create Toggle
        </a>
        <Modal show={this.state.showModal} onHide={close}>
          <Modal.Header closeButton>
            <Modal.Title>Create new Toggle</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            {this.renderMessage()}
            <form>
              <FormGroup controlId="toggleName">
                <ControlLabel>Name</ControlLabel>
                <FormControl
                  type="text"
                  placeholder="My-Toggle"
                  value={this.state.name}
                  onChange={e => this.setState({ name: e.target.value })}
                />
              </FormGroup>
              <FormGroup controlId="toggleDescription">
                <ControlLabel>Description</ControlLabel>
                <FormControl
                  type="text"
                  placeholder="some short description of the toggle"
                  value={this.state.description}
                  onChange={e => this.setState({ description: e.target.value })}
                />
              </FormGroup>
            </form>
          </Modal.Body>
          <Modal.Footer>
            <Button onClick={save} bsStyle="primary">
              Create
            </Button>
            <Button onClick={close}>Close</Button>
          </Modal.Footer>
        </Modal>
      </span>
    );
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(CreateToggle);
