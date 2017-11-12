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

    this.openModal = this.openModal.bind(this);
    this.closeModal = this.closeModal.bind(this);
    this.resetForm = this.resetForm.bind(this);

    this.renderMessage = this.renderMessage.bind(this);
    this.renderForm = this.renderForm.bind(this);

    this.state = {
      showModal: false,
      name: "",
      description: "",
      failureMessages: []
    };
  }

  openModal(e) {
    e.preventDefault();
    this.setState({ showModal: true });
  }

  closeModal() {
    this.resetForm();
    this.setState({ showModal: false });
  }

  resetForm() {
    this.setState({
      name: "",
      description: "",
      failureMessages: []
    });
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

  renderForm() {
    return (
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
    );
  }

  render() {
    const save = () =>
      this.props.createToggle(
        this.state.name,
        this.state.description,
        () => this.closeModal(),
        body => this.setState({ failureMessages: body.messages })
      );

    return (
      <span>
        <a href="#" onClick={this.openModal}>
          Create Toggle
        </a>
        <Modal show={this.state.showModal} onHide={this.closeModal}>
          <Modal.Header closeButton>
            <Modal.Title>Create new Toggle</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            {this.renderMessage()}
            {this.renderForm()}
          </Modal.Body>
          <Modal.Footer>
            <Button onClick={save} bsStyle="primary">
              Create
            </Button>
            <Button onClick={this.closeModal}>Close</Button>
          </Modal.Footer>
        </Modal>
      </span>
    );
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(CreateToggle);
